package ru.viqa.ui_testing.common.utils;

import ru.viqa.ui_testing.common.funcInterfaces.FuncT;

import static java.lang.System.currentTimeMillis;

/**
 * Created by 12345 on 28.09.2014.
 */
public class Timer {
    private long _timeoutInMSec = 10000;
    private long _retryTimeoutInMSec = 100;
    private final Long start = currentTimeMillis();

    public Timer() throws Exception { }
    public Timer(long timeoutInMSec, long retryTimeoutInMSec) throws Exception {
        this();
        _timeoutInMSec = timeoutInMSec;
        _retryTimeoutInMSec = retryTimeoutInMSec;
    }
    public Timer setTimeout(long timeoutInMSec) { _timeoutInMSec = timeoutInMSec; return this; }
    public Timer setRetryTimeout(long retryTimeoutInMSec) { _retryTimeoutInMSec = retryTimeoutInMSec; return this; }

    public Long timePassedInMSec() throws Exception {
        Long now = currentTimeMillis();
        return now - start;
    }

    public boolean timeoutPassed() throws Exception {
        return timePassedInMSec() >  _timeoutInMSec;
    }

    public boolean wait(FuncT<Boolean> waitCase) throws Exception {
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
