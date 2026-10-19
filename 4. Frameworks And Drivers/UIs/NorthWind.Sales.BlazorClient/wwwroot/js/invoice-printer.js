(function () {
    var PAGE_WIDTH = 595;
    var PAGE_HEIGHT = 842;
    var MARGIN = 42;
    var CONTENT_WIDTH = PAGE_WIDTH - MARGIN * 2;
    var NAVY = [0.059, 0.169, 0.282];
    var DARK = [0.12, 0.16, 0.18];
    var MUTED = [0.38, 0.42, 0.44];
    var BORDER = [0.83, 0.86, 0.87];
    var PALE = [0.92, 0.95, 0.98];
    var LIGHT = [0.97, 0.98, 0.98];
    var CP1252 = {
        8364: 128, 8218: 130, 402: 131, 8222: 132, 8230: 133, 8224: 134,
        8225: 135, 710: 136, 8240: 137, 352: 138, 8249: 139, 338: 140,
        381: 142, 8216: 145, 8217: 146, 8220: 147, 8221: 148, 8226: 149,
        8211: 150, 8212: 151, 732: 152, 8482: 153, 353: 154, 8250: 155,
        339: 156, 382: 158, 376: 159
    };

    function cleanText(value) {
        return (value || '').replace(/\s+/g, ' ').trim();
    }

    function childTextWithout(parent, childToRemove) {
        var copy = parent.cloneNode(true);
        var children = copy.querySelectorAll('.invoice-field-label');
        Array.prototype.forEach.call(children, function (child) { child.remove(); });
        return cleanText(copy.textContent);
    }

    function getSectionValues(section) {
        var values = {};
        if (!section) return values;
        Array.prototype.forEach.call(section.querySelectorAll('.invoice-field-row'), function (row) {
            var label = row.querySelector('.invoice-field-label');
            if (label) values[cleanText(label.textContent).replace(/:$/, '')] = childTextWithout(row, label);
        });
        return values;
    }

    function readInvoice(element) {
        var sections = element.querySelectorAll('.invoice-section-box');
        var customer = getSectionValues(sections[0]);
        var sale = getSectionValues(sections[1]);
        var number = cleanText((element.querySelector('.invoice-badge-num') || {}).textContent);
        var dateBlock = element.querySelector('.invoice-badge-card .small');
        var date = dateBlock ? cleanText(dateBlock.textContent).replace(/^.*?:\s*/, '') : '';
        var tableRows = element.querySelectorAll('.invoice-table tbody tr');
        var products = Array.prototype.map.call(tableRows, function (row, index) {
            var cells = row.querySelectorAll('td');
            return {
                number: String(index + 1),
                code: cleanText(cells[1] && cells[1].textContent).replace(/^#/, ''),
                description: cleanText(cells[2] && cells[2].textContent),
                quantity: cleanText(cells[3] && cells[3].textContent),
                unitPrice: cleanText(cells[4] && cells[4].textContent),
                subtotal: cleanText(cells[5] && cells[5].textContent)
            };
        });
        var totals = Array.prototype.map.call(element.querySelectorAll('.invoice-totals-row'), function (row) {
            var parts = row.children;
            return {
                label: cleanText(parts[0] && parts[0].textContent).replace(/:$/, ''),
                value: cleanText(parts[1] && parts[1].textContent),
                isTotal: row.classList.contains('invoice-total-highlight')
            };
        });
        var footer = element.querySelector('.invoice-legal-note');
        var footerLines = footer ? cleanText(footer.textContent).split(/\s{2,}/) : [];

        return {
            number: number.replace(/^.*?\s/, ''),
            date: date,
            customer: customer,
            sale: sale,
            products: products,
            totals: totals.filter(function (item) { return item.label && item.label !== 'Descuento Comercial (0%)'; }),
            thanks: footer && footer.querySelector('.fw-bold')
                ? cleanText(footer.querySelector('.fw-bold').textContent)
                : 'Gracias por su compra en Ventas NorthWind',
            reference: footer && footer.querySelector('.font-monospace')
                ? cleanText(footer.querySelector('.font-monospace').textContent)
                : footerLines.slice(1).join(' ')
        };
    }

    function rgb(color, fill) {
        return color.map(function (component) { return component.toFixed(3); }).join(' ') + (fill ? ' rg' : ' RG');
    }

    function escapePdfText(text) {
        var output = '';
        Array.from(String(text || '')).forEach(function (character) {
            var code = character.codePointAt(0);
            var byte = code <= 255 ? code : (CP1252[code] || 63);
            if (byte === 40 || byte === 41 || byte === 92) output += '\\' + String.fromCharCode(byte);
            else if (byte < 32 || byte > 126) output += '\\' + byte.toString(8).padStart(3, '0');
            else output += String.fromCharCode(byte);
        });
        return '(' + output + ')';
    }

    function measureText(text, size) {
        return Array.from(String(text || '')).reduce(function (width, character) {
            if (' il.,:;!|'.indexOf(character) >= 0) return width + size * 0.26;
            if ('MW@%&'.indexOf(character) >= 0) return width + size * 0.82;
            return width + size * 0.53;
        }, 0);
    }

    function wrapText(text, maxWidth, size) {
        var words = cleanText(text).split(' ');
        var lines = [];
        var line = '';
        words.forEach(function (word) {
            var candidate = line ? line + ' ' + word : word;
            if (line && measureText(candidate, size) > maxWidth) {
                lines.push(line);
                line = word;
            } else {
                line = candidate;
            }
        });
        if (line) lines.push(line);
        return lines.length ? lines : [''];
    }

    function createPage() {
        var commands = [];
        return {
            commands: commands,
            rect: function (x, top, width, height, color, stroke) {
                var y = PAGE_HEIGHT - top - height;
                if (color) commands.push(rgb(color, true));
                if (stroke) commands.push(rgb(stroke, false));
                commands.push(x.toFixed(2) + ' ' + y.toFixed(2) + ' ' + width.toFixed(2) + ' ' + height.toFixed(2) + ' re ' + (color && stroke ? 'B' : color ? 'f' : 'S'));
            },
            line: function (x1, top1, x2, top2, color, width) {
                commands.push(rgb(color || BORDER, false));
                commands.push((width || 0.6).toFixed(2) + ' w');
                commands.push(x1.toFixed(2) + ' ' + (PAGE_HEIGHT - top1).toFixed(2) + ' m ' + x2.toFixed(2) + ' ' + (PAGE_HEIGHT - top2).toFixed(2) + ' l S');
            },
            text: function (value, x, top, size, color, bold, align) {
                var text = cleanText(value);
                var width = measureText(text, size) * (bold ? 1.04 : 1);
                var actualX = align === 'right' ? x - width : align === 'center' ? x - width / 2 : x;
                commands.push('BT /' + (bold ? 'F2' : 'F1') + ' ' + size.toFixed(2) + ' Tf ' + rgb(color || DARK, true));
                commands.push('1 0 0 1 ' + actualX.toFixed(2) + ' ' + (PAGE_HEIGHT - top - size * 0.86).toFixed(2) + ' Tm ' + escapePdfText(text) + ' Tj ET');
            },
            wrapped: function (value, x, top, width, size, color, bold, leading, maxLines) {
                var lines = wrapText(value, width, size);
                if (maxLines && lines.length > maxLines) {
                    lines = lines.slice(0, maxLines);
                    lines[maxLines - 1] = lines[maxLines - 1].replace(/[.,;:]?$/, '...');
                }
                lines.forEach(function (line, index) {
                    this.text(line, x, top + index * (leading || size * 1.3), size, color, bold);
                }, this);
                return lines.length;
            }
        };
    }

    function drawBox(page, x, top, width, title) {
        page.rect(x, top, width, 82, [1, 1, 1], BORDER);
        page.text(title.toUpperCase(), x + 11, top + 10, 9, NAVY, true);
    }

    function drawFirstPageHeader(page, invoice) {
        page.rect(0, 0, PAGE_WIDTH, 130, NAVY);
        page.text('VENTAS NORTHWIND', MARGIN, 36, 24, [1, 1, 1], true);
        page.text('Factura electrónica', MARGIN, 72, 12, [0.80, 0.94, 0.92], false);
        page.text('Factura No. ' + invoice.number, PAGE_WIDTH - MARGIN, 38, 15, [1, 1, 1], true, 'right');
        page.text('Fecha: ' + invoice.date, PAGE_WIDTH - MARGIN, 60, 9.5, [1, 1, 1], false, 'right');

        var boxTop = 150;
        var boxWidth = 247;
        var rightX = MARGIN + boxWidth + 17;
        drawBox(page, MARGIN, boxTop, boxWidth, 'Facturar a');
        drawBox(page, rightX, boxTop, boxWidth, 'Detalles de la venta');

        var customer = invoice.customer;
        page.wrapped(customer['Cliente'] || customer['Nombre'] || 'Cliente', MARGIN + 11, boxTop + 27, boxWidth - 22, 11.5, DARK, true, 14, 1);
        var customerLines = [
            customer['ID / RUC / CC'] ? 'ID: ' + customer['ID / RUC / CC'] : '',
            customer['Dirección'] ? 'Dirección: ' + customer['Dirección'] : '',
            customer['Ciudad / País'] ? 'Ciudad: ' + customer['Ciudad / País'] : ''
        ].filter(Boolean);
        customerLines.slice(0, 3).forEach(function (line, index) {
            page.wrapped(line, MARGIN + 11, boxTop + 44 + index * 12, boxWidth - 22, 8.5, MUTED, false, 10, 1);
        });

        var seller = invoice.sale['Vendedor'] || 'Vendedor';
        page.wrapped('Vendedor: ' + seller, rightX + 11, boxTop + 29, boxWidth - 22, 9.5, DARK, false, 12, 2);
        page.wrapped('Moneda: ' + (invoice.sale['Moneda'] || 'USD'), rightX + 11, boxTop + 52, boxWidth - 22, 9.5, MUTED, false, 12, 1);
        page.wrapped('Estado: ' + (invoice.sale['Estado'] || 'Pagada'), rightX + 11, boxTop + 66, boxWidth - 22, 9.5, NAVY, true, 12, 1);
    }

    function drawContinuationHeader(page, invoice, pageNumber) {
        page.rect(0, 0, PAGE_WIDTH, 82, NAVY);
        page.text('VENTAS NORTHWIND', MARGIN, 25, 18, [1, 1, 1], true);
        page.text('Factura electrónica · ' + invoice.number, MARGIN, 52, 10, [0.80, 0.94, 0.92], false);
        page.text('Continuación · Página ' + pageNumber, PAGE_WIDTH - MARGIN, 38, 10, [1, 1, 1], false, 'right');
    }

    function drawTable(page, products, top) {
        var columns = [26, 48, 208, 48, 84, 95];
        var labels = ['#', 'Código', 'Descripción', 'Cant.', 'P. Unit.', 'Subtotal'];
        var x = MARGIN;
        page.rect(MARGIN, top, CONTENT_WIDTH, 29, NAVY);
        labels.forEach(function (label, index) {
            var align = index === 0 || index === 3 ? 'center' : index > 3 ? 'right' : 'left';
            var textX = align === 'center' ? x + columns[index] / 2 : align === 'right' ? x + columns[index] - 8 : x + 8;
            page.text(label, textX, top + 9, 8.5, [1, 1, 1], true, align);
            x += columns[index];
        });

        var rowTop = top + 29;
        products.forEach(function (product, index) {
            var descriptionLines = wrapText(product.description, columns[2] - 14, 8.5);
            var rowHeight = Math.max(27, descriptionLines.length * 10 + 12);
            if (index % 2 === 1) page.rect(MARGIN, rowTop, CONTENT_WIDTH, rowHeight, LIGHT);
            page.text(product.number, MARGIN + columns[0] / 2, rowTop + 9, 8.5, MUTED, false, 'center');
            page.text(product.code, MARGIN + columns[0] + 7, rowTop + 9, 8, MUTED, false);
            page.wrapped(product.description, MARGIN + columns[0] + columns[1] + 7, rowTop + 7, columns[2] - 14, 8.5, DARK, false, 10, 3);
            page.text(product.quantity, MARGIN + columns[0] + columns[1] + columns[2] + columns[3] / 2, rowTop + 9, 8.5, DARK, false, 'center');
            var unitRight = MARGIN + columns.slice(0, 5).reduce(function (a, b) { return a + b; }, 0) - 7;
            page.text(product.unitPrice, unitRight, rowTop + 9, 8.5, DARK, false, 'right');
            page.text(product.subtotal, PAGE_WIDTH - MARGIN - 7, rowTop + 9, 8.5, NAVY, true, 'right');
            rowTop += rowHeight;
            page.line(MARGIN, rowTop, PAGE_WIDTH - MARGIN, rowTop, BORDER, 0.45);
        });
        return rowTop;
    }

    function drawTotals(page, totals, top) {
        var boxWidth = 268;
        var x = PAGE_WIDTH - MARGIN - boxWidth;
        var filtered = totals.filter(function (item) { return item.label !== 'Descuento Comercial (0%)'; });
        var totalIndex = filtered.findIndex(function (item) { return item.isTotal; });
        var normalItems = totalIndex >= 0 ? filtered.slice(0, totalIndex) : filtered.slice(0, -1);
        var grandTotal = totalIndex >= 0 ? filtered[totalIndex] : filtered[filtered.length - 1];
        normalItems.forEach(function (item, index) {
            var y = top + index * 23;
            page.text(item.label + ':', x, y + 4, 9, MUTED, true);
            page.text(item.value, x + boxWidth, y + 4, 9, DARK, false, 'right');
            page.line(x, y + 19, x + boxWidth, y + 19, BORDER, 0.45);
        });
        var totalTop = top + normalItems.length * 23 + 4;
        page.rect(x, totalTop, boxWidth, 38, PALE);
        page.text(grandTotal ? grandTotal.label.toUpperCase() + ':' : 'TOTAL:', x + 10, totalTop + 11, 13, NAVY, true);
        page.text(grandTotal ? grandTotal.value : '', x + boxWidth - 10, totalTop + 11, 13, NAVY, true, 'right');
        return totalTop + 38;
    }

    function totalsHeight(totals) {
        var filtered = totals.filter(function (item) { return item.label !== 'Descuento Comercial (0%)'; });
        var normalCount = Math.max(0, filtered.length - 1);
        return normalCount * 23 + 42;
    }

    function drawFooter(page, invoice, pageNumber, pageCount) {
        page.line(MARGIN, 792, PAGE_WIDTH - MARGIN, 792, [0.34, 0.72, 0.66], 0.7);
        page.text(invoice.thanks, PAGE_WIDTH / 2, 802, 9, NAVY, false, 'center');
        if (invoice.reference) page.text(invoice.reference, PAGE_WIDTH / 2, 815, 6.5, MUTED, false, 'center');
        page.text('Página ' + pageNumber + ' de ' + pageCount, PAGE_WIDTH / 2, 828, 7, MUTED, false, 'center');
    }

    function drawInvoice(invoice) {
        var products = invoice.products.length ? invoice.products : [{ number: '1', code: '', description: 'Sin productos', quantity: '', unitPrice: '', subtotal: '' }];
        var chunks = [];
        for (var i = 0; i < products.length; i += 13) chunks.push(products.slice(i, i + 13));
        var needsTotalsPage = false;
        var pages = chunks.map(function (chunk, index) {
            var page = createPage();
            var tableTop;
            if (index === 0) {
                drawFirstPageHeader(page, invoice);
                tableTop = 252;
            } else {
                drawContinuationHeader(page, invoice, index + 1);
                tableTop = 112;
            }
            var tableBottom = drawTable(page, chunk, tableTop);
            if (index === chunks.length - 1) {
                if (tableBottom + 20 + totalsHeight(invoice.totals) > 780) needsTotalsPage = true;
                else drawTotals(page, invoice.totals, tableBottom + 20);
            }
            return page;
        });

        if (needsTotalsPage) {
            var totalPage = createPage();
            drawContinuationHeader(totalPage, invoice, pages.length + 1);
            drawTotals(totalPage, invoice.totals, 125);
            pages.push(totalPage);
        }

        pages.forEach(function (page, index) { drawFooter(page, invoice, index + 1, pages.length); });
        return pages;
    }

    function latin1Bytes(text) {
        var result = new Uint8Array(text.length);
        for (var i = 0; i < text.length; i++) result[i] = text.charCodeAt(i) & 255;
        return result;
    }

    function concatenate(chunks) {
        var length = chunks.reduce(function (total, item) { return total + item.length; }, 0);
        var result = new Uint8Array(length);
        var offset = 0;
        chunks.forEach(function (chunk) { result.set(chunk, offset); offset += chunk.length; });
        return result;
    }

    function buildPdf(pages) {
        var objects = [];
        var pageIds = pages.map(function (_, index) { return 5 + index * 2; });
        objects[1] = '<< /Type /Catalog /Pages 2 0 R >>';
        objects[2] = '<< /Type /Pages /Kids [' + pageIds.map(function (id) { return id + ' 0 R'; }).join(' ') + '] /Count ' + pages.length + ' >>';
        objects[3] = '<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>';
        objects[4] = '<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica-Bold /Encoding /WinAnsiEncoding >>';

        pages.forEach(function (page, index) {
            var pageId = 5 + index * 2;
            var streamId = pageId + 1;
            var stream = page.commands.join('\n');
            objects[pageId] = '<< /Type /Page /Parent 2 0 R /MediaBox [0 0 ' + PAGE_WIDTH + ' ' + PAGE_HEIGHT + '] /Resources << /Font << /F1 3 0 R /F2 4 0 R >> >> /Contents ' + streamId + ' 0 R >>';
            objects[streamId] = '<< /Length ' + latin1Bytes(stream).length + ' >>\nstream\n' + stream + '\nendstream';
        });

        var chunks = [latin1Bytes('%PDF-1.4\n%\xE2\xE3\xCF\xD3\n')];
        var offsets = [0];
        var offset = chunks[0].length;
        for (var objectId = 1; objectId < objects.length; objectId++) {
            if (!objects[objectId]) continue;
            offsets[objectId] = offset;
            var objectBytes = latin1Bytes(objectId + ' 0 obj\n' + objects[objectId] + '\nendobj\n');
            chunks.push(objectBytes);
            offset += objectBytes.length;
        }

        var xrefOffset = offset;
        var xref = 'xref\n0 ' + objects.length + '\n0000000000 65535 f \n';
        for (var i = 1; i < objects.length; i++) xref += String(offsets[i]).padStart(10, '0') + ' 00000 n \n';
        xref += 'trailer\n<< /Size ' + objects.length + ' /Root 1 0 R >>\nstartxref\n' + xrefOffset + '\n%%EOF';
        chunks.push(latin1Bytes(xref));
        return new Blob([concatenate(chunks)], { type: 'application/pdf' });
    }

    function downloadFallback(blob, fileName) {
        var url = URL.createObjectURL(blob);
        var link = document.createElement('a');
        link.href = url;
        link.download = fileName;
        link.style.display = 'none';
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.setTimeout(function () { URL.revokeObjectURL(url); }, 1000);
    }

    window.invoicePdfHelper = {
        downloadPdf: function (elementId, fileName) {
            var original = document.getElementById(elementId);
            if (!original) {
                console.error('No se encontró la factura para exportar:', elementId);
                return Promise.resolve(false);
            }

            try {
                var invoice = readInvoice(original);
                var blob = buildPdf(drawInvoice(invoice));
                var suggestedName = fileName || 'Factura_NorthWind.pdf';
                if (window.showSaveFilePicker) {
                    return window.showSaveFilePicker({
                        suggestedName: suggestedName,
                        types: [{ description: 'Documento PDF', accept: { 'application/pdf': ['.pdf'] } }]
                    }).then(function (handle) {
                        return handle.createWritable().then(function (writable) {
                            return writable.write(blob).then(function () { return writable.close(); });
                        });
                    }).then(function () {
                        return true;
                    }).catch(function (error) {
                        if (error && error.name === 'AbortError') return false;
                        console.error('No se pudo guardar con el selector de archivos; se descargará el PDF:', error);
                        downloadFallback(blob, suggestedName);
                        return true;
                    });
                }

                downloadFallback(blob, suggestedName);
                return Promise.resolve(true);
            } catch (error) {
                console.error('Error al generar el PDF de la factura:', error);
                return Promise.resolve(false);
            }
        }
    };
})();
