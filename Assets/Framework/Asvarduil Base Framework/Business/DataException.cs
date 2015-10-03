using System;

public class DataException : Exception
{
    #region Constructor

    public DataException() : base()
    {

    }

    public DataException(string message) : base(message)
    {
    }

    #endregion Constructor
}

