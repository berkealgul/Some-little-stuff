class CountDown
{
    int durationSeconds;
    int remainingTime;

    public CountDown(int duration)
    {
        // add millis/1000 offset for recording starting time
        durationSeconds = duration+(millis()/1000);
        remainingTime = durationSeconds;
    }

    // updates remaining time
    void tick() 
    { 
        //millis() processing command, returns time in 1000ths sec since program started
        remainingTime = max(0, durationSeconds - millis()/1000);
    }
}
