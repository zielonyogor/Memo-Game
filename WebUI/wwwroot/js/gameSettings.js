document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("setupForm");
    const rowsInput = document.getElementById("inputRows");
    const colsInput = document.getElementById("inputCols");
    const statusLabel = document.getElementById("gridStatus");
    const errorMsg = document.getElementById("errorMsg");
    const btnStart = document.getElementById("btnStart");

    const maxUniqueCards = parseInt(form.dataset.maxCards) || 0;

    function updateStatus() {
        const r = parseInt(rowsInput.value) || 1;
        const c = parseInt(colsInput.value) || 1;
        const totalCells = r * c;
        const pairsNeeded = Math.floor(totalCells / 2);

        statusLabel.innerText = `Grid: ${totalCells} cells (${pairsNeeded} pairs)`;
        let isValid = true;

        if (pairsNeeded > maxUniqueCards) {
            isValid = false;
            errorMsg.innerText = `Need ${pairsNeeded} pairs, but library only has ${maxUniqueCards}!`;
            errorMsg.classList.remove("d-none");
        }
        else if (pairsNeeded === 0) {
            isValid = false;
            errorMsg.innerText = `There must be at least one pair!`;
            errorMsg.classList.remove("d-none");
        }
        else {
            errorMsg.classList.add("d-none");
        }

        btnStart.disabled = !isValid;
    }

    rowsInput.addEventListener("input", updateStatus);
    colsInput.addEventListener("input", updateStatus);
    updateStatus();
});