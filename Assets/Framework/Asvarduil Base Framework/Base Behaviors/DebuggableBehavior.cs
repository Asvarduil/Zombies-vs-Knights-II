using UnityEngine;
using System;

public abstract class DebuggableBehavior : MonoBehaviour
{
    #region Enumerations

    public enum LogLevel
    {
        Info,
        Warn,
        Error,
    }

    #endregion Enumerations

    #region Variables / Properties

    public bool DebugMode = false;
    public bool ShowTimestamps = false;

    #endregion Variables / Properties

    #region Methods

    public void DebugMessage(string message, LogLevel level = LogLevel.Info)
    {
        if (!DebugMode)
            return;

        if (ShowTimestamps)
            message = DateTime.Now.ToString("HH:mm:ss") + ": " + message;

        switch (level)
        {
            case LogLevel.Info:
                Debug.Log(message);
                break;

            case LogLevel.Warn:
                Debug.LogWarning(message);
                break;

            case LogLevel.Error:
                Debug.LogError(message);
                break;

            default:
                throw new Exception("Unexpected log level! " + level.ToString() + Environment.NewLine + " message: " + message);
        }
    }

    public void FormattedDebugMessage(LogLevel level, string messageFormat, params object[] messageArgs)
    {
        if (!DebugMode)
            return;

        // TODO: Fix this if possible.
        if (ShowTimestamps)
            messageFormat = DateTime.Now.ToString("HH:mm:ss") + ": " + messageFormat;

        string message = string.Format(messageFormat, messageArgs);

        switch (level)
        {
            case LogLevel.Info:
                Debug.Log(message);
                break;

            case LogLevel.Warn:
                Debug.LogWarning(message);
                break;

            case LogLevel.Error:
                Debug.LogError(message);
                break;

            default:
                throw new Exception("Unexpected log level! " + level.ToString() + Environment.NewLine + " message: " + message);
        }
    }

    #endregion Methods
}
