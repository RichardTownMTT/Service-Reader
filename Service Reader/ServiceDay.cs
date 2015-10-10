using System;

public class ServiceDay
{
    private DateTime dtServiceEntry = new DateTime();
    private DateTime travelStartTime = new DateTime();
    private DateTime arrivalTimeOnsite = new DateTime();
    private DateTime departureTimeSite = new DateTime();
    private DateTime travelEndTime = new DateTime();
    private int mileage = 0;
    private int dailyAllowance = 0;
    private int overnightAllowance = 0;
    private double totalTraveTime = 0;
    private double totalTimeOnsite = 0;
    private string dailyReport = "";
    private string partsSuppliedToday = "";

    public ServiceDay()
    {

    }
}
