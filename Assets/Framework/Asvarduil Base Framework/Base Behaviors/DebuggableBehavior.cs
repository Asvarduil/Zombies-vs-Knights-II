using UnityEngine;
using System;

public abstract class DebuggableBehavior : MonoBehaviour
{
    #region Enumerations

    public enum LogLevel
    {
        Information,
        Warning,
        LogicError,
    }

    #endregion Enumerations

    #region Variables / Properties

    public bool DebugMode = false;
    public bool ShowTimestamps = false;

    #endregion Variables / Properties

    #region Methods

    public void DebugMessage(string message, LogLevel level = LogLevel.Information)
    {
        if (!DebugMode)
            return;

        if (ShowTimestamps)
            message = DateTime.Now.ToString("HH:mm:ss") + ": " + message;

        switch (level)
        {
            case LogLevel.Information:
                Debug.Log(message);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(message);
                break;

            case LogLevel.LogicError:
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
            case LogLevel.Information:
                Debug.Log(message);
                break;

            case LogLevel.Warning:
                Debug.LogWarning(message);
                break;

            case LogLevel.LogicError:
                Debug.LogError(message);
                break;

            default:
                throw new Exception("Unexpected log level! " + level.ToString() + Environment.NewLine + " message: " + message);
        }
    }

    #endregion Methods
}
