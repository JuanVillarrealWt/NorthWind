document.addEventListener("beforeinput", event => {
    const input = event.target;
    if (!(input instanceof HTMLInputElement)) return;

    if (input.dataset.numericOnly === "true") {
        const isInsertion = event.inputType.startsWith("insert");
        let insertedText = event.data;

        if (event.inputType === "insertFromPaste" && event.dataTransfer) {
            insertedText = event.dataTransfer.getData("text/plain");
        }

        if (isInsertion && (!insertedText || !/^[0-9]+$/.test(insertedText))) {
            event.preventDefault();
            return;
        }

        const selectionStart = input.selectionStart ?? input.value.length;
        const selectionEnd = input.selectionEnd ?? selectionStart;
        let start = selectionStart;
        let end = selectionEnd;

        if (start === end && event.inputType === "deleteContentBackward") start = Math.max(0, start - 1);
        if (start === end && event.inputType === "deleteContentForward") end = Math.min(input.value.length, end + 1);

        const nextValue = input.value.slice(0, start) + (isInsertion ? insertedText : "") + input.value.slice(end);
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
