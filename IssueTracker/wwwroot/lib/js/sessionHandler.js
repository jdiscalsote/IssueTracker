let inactivityTimeout;
let warningTimeout;
let warningPopup;

function resetInactivityTimer() {
    clearTimeout(inactivityTimeout);
    clearTimeout(warningTimeout);

    if (warningPopup) {
        warningPopup.close();
        warningPopup = null;
    }

    inactivityTimeout = setTimeout(() => {
        showWarningPopup();
    }, 60000); // 1 minute

    warningTimeout = setTimeout(() => {
        endSession();
    }, 120000); // 2 minutes
}

function showWarningPopup() {
    let remainingTime = 60; // 60 seconds
    warningPopup = new Notification("Are you there?", {
        body: `If not, we'll close this session in ${remainingTime} seconds.`,
        requireInteraction: true,
    });

    warningPopup.onclick = () => {
        resetInactivityTimer();
        window.location.href = '/Dashboard/Dashboard';
    };

    const interval = setInterval(() => {
        if (remainingTime > 0) {
            remainingTime--;
            warningPopup.body = `If not, we'll close this session in ${remainingTime} seconds.`;
        } else {
            clearInterval(interval);
        }
    }, 1000);
}

function endSession() {
    if (warningPopup) {
        warningPopup.close();
    }

    alert("Session ended. There was no activity for a while, so we closed the session.");

    fetch('/Login/Logout', { method: 'POST' })
        .then(() => {
            window.location.href = '/Login/Index';
        });
}

window.onload = resetInactivityTimer;
document.onmousemove = resetInactivityTimer;
document.onkeydown = resetInactivityTimer;

if (Notification.permission !== "granted") {
    Notification.requestPermission().then(permission => {
        if (permission !== "granted") {
            alert("Please enable notifications for session management.");
        }
    });
}
