using FixWithCustomSerialization.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace FixWithCustomSerialization.Controllers;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IDbService _mongoDb;

    public MediaController(ILogger<MediaController> logger, IDbService _db)
    {
        _logger = logger;
        _mongoDb = _db;
    }

    [HttpPost]
    public async Task<MediaFile> UpdateMedia([FromBody] MediaFile media)
    {
        /* todo: Implement the method
         * 
         * 1. media will be contained in the ImageDataB64 prop. It should have been already saved to the webroots folder by the "read" method of MediaFileJsonConverter
         * 2. Save the media object in Mongo db
         * 3. return the MediaFile object
         * 4. We expect the "write" method of MediaFileJsonConverter to create the public URL
         * 
         * */

        await _mongoDb.CreateAsync(media);

        return await _mongoDb.GetAsync(media.Name);
    }

    [HttpGet("{Name}")]
    public async Task<MediaFile> GetMedia(string Name)
    {
        /*
         * return _mongoDb.getcollection<MediaFile>.Find(m=>m.Name==Name).SingleAsync();
         * 
         * 1. We expect the "write" method of MediaFileJsonConverter to create the public URL
         * 
         */

        return await _mongoDb.GetAsync(Name);
    }
}

/// <summary>
/// todo: implement MediaFileJsonConverter  
/// [JsonConverter(typeof(MediaFileJsonConverter))]
/// </summary>
[JsonConverter(typeof(MediaFileJsonConverter))]
public class MediaFile
{
    /// <summary>
    /// todo: We want media name to be Unique, Please enforce using MongoDb Unique Index
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// todo: donot Save in database
    /// </summary>
    [BsonIgnore]
    public string ImageDataB64 { get; set; } = "";

    /// <summary>
    /// todo: donot Save in database, Generate dynamically using 
    /// </summary>
    [BsonIgnore]
    public string PublicUrl { get; set; } = "";
}



