
    let timer;
    isWorkSession = true;
    let isRunning = false;
    let timeLeft = 25; // 25 minutes in seconds
    let completedWorkSessions = 0;

    const timerDisplay = document.getElementById('timerDisplay');
    const sessionDisplay = document.getElementById('sessionDisplay');
    const startButton = document.getElementById('startButton');
    const skipButton = document.getElementById('skipButton');
    const resetButton = document.getElementById('resetButton');


    const alarmFinishSoundPath = '/sounds/alarm_finished/'; // Path to alarm finish sound file
    const alarmTickSoundPath = '/sounds/alarm_ticks/'; // Path to alarm tick sound file
    const alarmFinishSound = new Audio(alarmFinishSoundPath + 'musical_alarm' + '.mp3'); // Path to alarm sound file
    const alarmTickSound = new Audio(alarmTickSoundPath + 'lo-fi' + '.mp3'); // Path to alarm sound file
    startButton.addEventListener('click', function() {
        if (!isRunning) {
            isRunning = true;
            startButton.textContent = 'Stop';
            timer = setInterval(updateTimer, 1000);

        }
        else {
            isRunning = false;
            alarmTickSound.pause();
            startButton.textContent = 'Start';
            clearInterval(timer);
        }
    });

    function updateTimer() {
        if (timeLeft <= 0) {
            alarmTickSound.pause();
            alarmFinishSound.play();
            if (isWorkSession) {
                completedWorkSessions++;
                sessionDisplay.textContent = (completedWorkSessions % 4) + "/4";

                if (completedWorkSessions % 4 === 0) {
                    setBreak(true);
                } else {
                    setBreak(false);
                }

            } else {
                timeLeft = 25; // Reset to 25 minutes for the next work session
                isWorkSession = true;
            }

        }
        else {
            timeLeft = timeLeft - 1;
            alarmTickSound.play();
            min = Math.floor(timeLeft / 60);
            sec = timeLeft % 60;
            timerDisplay.textContent = `${min}:${sec < 10 ? '0' : ''}${sec}`;
        }
}

function setBreak(longBreak) {
    if (longBreak === true) {
        timeLeft = 15; // 15 minutes break
    } else {
        timeLeft = 5; // 5 minutes break
    }

    isWorkSession = false;
    console.log(completedWorkSessions);
}
skipButton.addEventListener('click', function skip() {
    timeLeft = 0;
    min = 0;
    sec = 0;
    timerDisplay.textContent = `${min}:${sec < 10 ? '0' : ''}${sec}`;
})
resetButton.addEventListener('click', function reset() {
    timeLeft = 0;
    min = 0;
    sec = 0;
    timerDisplay.textContent = `${min}:${sec < 10 ? '0' : ''}${sec}`;
    completedWorkSessions = 0;
})