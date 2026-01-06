let isProcessing = false;
let timerInterval;

document.addEventListener("DOMContentLoaded", function() {
    startTimer();
});

function startTimer() {
    timerInterval = setInterval(async () => {
        try {
            const response = await fetch('/Game/GetTime');
            if (response.ok) {
                const data = await response.json();
                const timerElement = document.getElementById('timer');
                if (timerElement) {
                    timerElement.innerText = data.time;
                }
            }
        } catch (error) {
            console.error("Timer error:", error);
        }
    }, 1000);
}

async function flipCard(element, row, col) {
    const cardObj = document.getElementById(`card-${row}-${col}`);
    const imgObj = document.getElementById(`img-${row}-${col}`);

    // Safety: Prevent double clicks or clicking already revealed cards
    if (isProcessing || cardObj.classList.contains('is-flipped')) return;

    isProcessing = true;

    try {
        const response = await fetch(`/Game/Flip?row=${row}&col=${col}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        });

        if (!response.ok) throw new Error("Server error");
        const data = await response.json();

        if (data.imageUrl) {
            imgObj.src = data.imageUrl;
            imgObj.style.display = "block";
            void cardObj.offsetWidth;
            cardObj.classList.add('is-flipped');
        }

        if (data.status === "match") {
            cardObj.classList.add('is-matched');
            const otherCard = document.querySelector('.is-flipped:not(.is-matched):not([id="' + cardObj.id + '"])');
            if (otherCard) otherCard.classList.add('is-matched');

            isProcessing = false;
        }
        else if (data.status === "wait") {
            isProcessing = false;
        }
        else if (data.status === "mismatch") {
            // MISMATCH hide card for a 1 sec

            setTimeout(() => {
                unflipCard(cardObj, imgObj);
                const otherCard = document.querySelector('.is-flipped:not(.is-matched)');
                if (otherCard) {
                    const otherImgId = otherCard.id.replace('card-', 'img-');
                    const otherImg = document.getElementById(otherImgId);
                    unflipCard(otherCard, otherImg);
                }

                isProcessing = false;
            }, 1000);
        }

        if (data.isFinished) {
            clearInterval(timerInterval);
            setTimeout(() => {
                window.location.href = "/Game/Result";
            }, 500);
        }

    } catch (err) {
        console.error("Flip failed:", err);
        isProcessing = false;
    }
}

function unflipCard(cardNode, imgNode) {
    cardNode.classList.remove('is-flipped');
    setTimeout(() => {
        if (imgNode) {
            imgNode.src = "";
            imgNode.style.display = "none";
        }
    }, 300);
}