using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public enum BlobSource
{
    Local,
    Remote,
    Raw
}

public abstract class JsonBlobLoaderBase<T> : DebuggableBehavior
    where T : IJsonSavable
{
    #region Variables / Properties

    public BlobSource Source;

    public TextAsset LocalBlob;
    public string RemoteBlobUrl;
    public string RawBlob;
    public List<T> Contents;

    protected IMapper<T> _mapper;
    protected JSONNode _mappedJson;

    public bool HasLoaded
    {
        get; private set;
    }

    #endregion Variables / Properties

    #region Methods

    protected void ReadBlobFromSource()
    {
        switch (Source)
        {
            case BlobSource.Local:
                RawBlob = FetchLocalBlob();
                _mappedJson = JSON.Parse(RawBlob);
                break;

            case BlobSource.Remote:
                StartCoroutine(DownloadBlob());
                break;

            case BlobSource.Raw:
                // Intentional stub; we're using "hardcoded" blob data for dev purposes.
                break;

            default:
                throw new InvalidOperationException("Unexpected Blob Source: " + Source);
        }
    }

    protected IEnumerator DownloadBlob()
    {
        WWW dataInterface = new WWW(RemoteBlobUrl);
        while (!dataInterface.isDone)
            yield return 0;

        RawBlob = dataInterface.text;
        _mappedJson = JSON.Parse(RawBlob);

        // TODO: Check that the blob has not been corrupted.
        //       If it has, set the raw blob to empty.

        dataInterface.Dispose();
    }

    protected string FetchLocalBlob()
    {
        if (LocalBlob == null)
            return string.Empty;

        return LocalBlob.text;
    }

    public virtual void MapJsonBlob()
    {
        ReadBlobFromSource();
        Contents = _mapper.MapJsonToList(_mappedJson);

        FormattedDebugMessage(LogLevel.Information,
            "Contents have loaded!  Count: {0}",
            Contents.Count);

        HasLoaded = true;
    }

    #endregion Methods
}
