const MAX_INPUT_LENGTH = 50;

function isTextualInput(input) {
    if (input instanceof HTMLTextAreaElement) return true;
    if (!(input instanceof HTMLInputElement)) return false;

    return ["text", "email", "password", "search", "tel", "url"].includes(input.type);
}

function applyDefaultMaxLength(element) {
    if (!(element instanceof HTMLInputElement || element instanceof HTMLTextAreaElement)) return;
    if (isTextualInput(element)) {
        if (element.maxLength < 0 || element.maxLength > MAX_INPUT_LENGTH) {
            element.maxLength = MAX_INPUT_LENGTH;
        }
    }
}

function applyMaxLengthToTree(root) {
    if (root instanceof HTMLInputElement || root instanceof HTMLTextAreaElement) {
        applyDefaultMaxLength(root);
    }
    root.querySelectorAll?.("input, textarea").forEach(applyDefaultMaxLength);
}

applyMaxLengthToTree(document);
new MutationObserver(records => {
    for (const record of records) {
        record.addedNodes.forEach(node => {
            if (node instanceof Element) applyMaxLengthToTree(node);
        });
    }
}).observe(document.documentElement, { childList: true, subtree: true });

document.addEventListener("beforeinput", event => {
    const input = event.target;
    if (!(input instanceof HTMLInputElement || input instanceof HTMLTextAreaElement)) return;

    const isInsertion = event.inputType.startsWith("insert");
    let insertedText = event.data ?? "";
    if (event.inputType === "insertFromPaste") {
        insertedText = event.dataTransfer?.getData("text/plain")
            ?? event.clipboardData?.getData("text/plain")
            ?? insertedText;
    }

    const supportsSelection = isTextualInput(input);
    const selectionStart = supportsSelection ? (input.selectionStart ?? input.value.length) : input.value.length;
    const selectionEnd = supportsSelection ? (input.selectionEnd ?? selectionStart) : selectionStart;
    let start = selectionStart;
    let end = selectionEnd;

    if (start === end && event.inputType === "deleteContentBackward") start = Math.max(0, start - 1);
    if (start === end && event.inputType === "deleteContentForward") end = Math.min(input.value.length, end + 1);

    const nextValue = input.value.slice(0, start) + (isInsertion ? insertedText : "") + input.value.slice(end);
    const effectiveMaxLength = input.maxLength >= 0
        ? Math.min(input.maxLength, MAX_INPUT_LENGTH)
        : MAX_INPUT_LENGTH;

    if (nextValue.length > effectiveMaxLength) {
        event.preventDefault();
        return;
    }

    if (input.dataset.alphaOnly === "true" && isInsertion && !/^[\p{L} ]*$/u.test(insertedText)) {
        event.preventDefault();
        return;
    }

    if (input.dataset.noSpaces === "true" && isInsertion && /\s/u.test(insertedText)) {
        event.preventDefault();
        return;
    }

    const numericOnly = input.dataset.numericOnly === "true";
    const decimalOnly = input.dataset.decimalOnly === "true";
    const alphaNumericOnly = input.dataset.alphanumericOnly === "true";

    if (numericOnly || decimalOnly || alphaNumericOnly) {
        const insertedTextIsValid = numericOnly
            ? /^[0-9]+$/.test(insertedText)
            : decimalOnly
                ? /^[0-9.]+$/.test(insertedText)
                : /^[A-Za-z0-9]+$/.test(insertedText);

        if (isInsertion && (!insertedText || !insertedTextIsValid)) {
            event.preventDefault();
            return;
        }

        if (decimalOnly && !/^\d*(?:\.\d{0,2})?$/.test(nextValue)) {
            event.preventDefault();
            return;
        }

        if (alphaNumericOnly && !/^[A-Za-z0-9]*$/.test(nextValue)) {
            event.preventDefault();
            return;
        }

        if (!numericOnly) return;

        const minimum = Number(input.dataset.minimum ?? "0");

        if ((nextValue.length === 0 && minimum > 0) || (nextValue.length > 0 && Number(nextValue) < minimum)) {
            event.preventDefault();
        }
        return;
    }

    if (input.dataset.trimLeading === "true" && event.inputType.startsWith("insert")) {
        const insertedText = event.data ?? event.dataTransfer?.getData("text/plain") ?? "";
        const selectionStart = input.selectionStart ?? 0;
        const selectionEnd = input.selectionEnd ?? selectionStart;
        const textBeforeCursor = input.value.slice(0, selectionStart);

        if (textBeforeCursor.trim().length === 0 && /^\s/.test(insertedText)) {
            event.preventDefault();
        }
    }
}, true);

document.addEventListener("input", event => {
    const input = event.target;
    if (!(input instanceof HTMLInputElement) || input.dataset.noSpaces !== "true" || !/\s/u.test(input.value)) return;

    const cursor = input.selectionStart ?? input.value.length;
    const removedBeforeCursor = (input.value.slice(0, cursor).match(/\s/gu) ?? []).length;
    input.value = input.value.replace(/\s/gu, "");
    const nextCursor = Math.max(0, cursor - removedBeforeCursor);
    input.setSelectionRange(nextCursor, nextCursor);
}, true);
