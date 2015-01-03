package ru.viqa.ui_testing.common.utils;

import ru.viqa.ui_testing.common.funcInterfaces.FuncT;

/**
 * Created by 12345 on 28.09.2014.
 */
public class Timer {
    private long _timeoutInMSec = 10000;
    private long _retryTimeoutInMSec = 100;
    private final Long start = System.currentTimeMillis();

    public Timer() { }
    public Timer(long timeoutInMSec, long retryTimeoutInMSec)
    {
        this();
        _timeoutInMSec = timeoutInMSec;
        _retryTimeoutInMSec = retryTimeoutInMSec;
    }
    public Timer setTimeout(long timeoutInMSec) { _timeoutInMSec = timeoutInMSec; return this; }
    public Timer setRetryTimeout(long retryTimeoutInMSec) { _retryTimeoutInMSec = retryTimeoutInMSec; return this; }

    public Long timePassedInMSec()
    {
        Long now = System.currentTimeMillis();
        return now - start;
    }

    public boolean timeoutPassed()
    {
        return timePassedInMSec() >  _timeoutInMSec;
    }

    public boolean wait(FuncT<Boolean> waitCase) throws InterruptedException {
        while (!timeoutPassed()) {
            if (TryGetResult(waitCase))
                return true;
            Thread.sleep(_retryTimeoutInMSec);
        }
        return false;
    }

    private static boolean TryGetResult(FuncT<Boolean> waitCase)
    {
        try { return waitCase.invoke(); }
        catch(Exception ex) { return false; }
    }
}
