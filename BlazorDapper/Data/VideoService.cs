﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BlazorDapper.Data
{
    public class VideoService : IVideoService
    {
        private IDbConnection db;
        private readonly SqlConnectionConfiguration _configuration;
        private readonly IConfiguration config;

        public VideoService(SqlConnectionConfiguration configuration, IConfiguration config)
        {
            this._configuration = configuration;
            this.config = config;
            //db = new SqlConnection(configuration.Value);
            db = new SqlConnection(config.GetConnectionString("SqlDbConnection"));
        }

        public async Task<bool> VideoInsert(Video video)
        {
            //using (var con = new SqlConnection(_configuration.Value))
            //{
            //}
            var parms = new DynamicParameters();
            parms.Add("Title", video.Title, DbType.String);
            parms.Add("DatePublished", video.Datepublished, DbType.Date);
            parms.Add("IsActive", video.IsActive, DbType.Boolean);
            //Insert by stored procedure
            //await con.ExecuteAsync("spVideo_Insert", parms, commandType: CommandType.StoredProcedure);

            //Raw Sql Query
            const string query = "INSERT INTO Video (Title, DatePublished, IsActive) VALUES (@Title, @DatePublished, @IsActive)";
            await db.ExecuteAsync(query, new { @Title = video.Title, video.Datepublished, video.IsActive }, commandType: CommandType.Text);
            //await db.ExecuteAsync(query, parms, commandType: CommandType.Text);
            return true;
        }
        public async Task<IEnumerable<Video>> GetAllVideos()
        {
            using (var con = new SqlConnection(_configuration.Value))
            {
                //Get All by stored procedure
                return await con.QueryAsync<Video>("spVideo_GetAll", commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<IEnumerable<Video>> GetAllVideosActive()
        {
            using (var con = new SqlConnection(_configuration.Value))
            {
                return await con.QueryAsync<Video>("spVideo_GetAllActive", commandType: CommandType.StoredProcedure);
            }
        }
        public async Task<Video> GetVideoById(int videoId)
        {
            var parms = new DynamicParameters();
            parms.Add("Videoid", videoId, DbType.Int32);
            using (var con = new SqlConnection(_configuration.Value))
            {
                return await con.QueryFirstOrDefaultAsync<Video>("spVideo_GetVideoById", parms, commandType: CommandType.StoredProcedure);
                //return await con.QueryFirstOrDefaultAsync<Video>("spVideo_GetVideoById", new { @VideoId = videoId }, commandType: CommandType.StoredProcedure);
            }
            //var sql = "SELECT * FROM Video WHERE VideoId = @VideoId";
            //return db.QueryFirstOrDefaultAsync<Video>(sql, new { @VideoId = videoId });
        }
        public async Task<bool> UpdateVideo(Video video)
        {
            using (var con = new SqlConnection(_configuration.Value))
            {
                var parms = new DynamicParameters();
                parms.Add("VideoId", video.VideoId, DbType.Int32);
                parms.Add("Title", video.Title, DbType.String);
                parms.Add("DatePublished", video.Datepublished, DbType.Date);
                parms.Add("IsActive", video.IsActive, DbType.Boolean);
                await con.ExecuteAsync("spVideo_UpdateVideo", parms, commandType: CommandType.StoredProcedure);

            }
            return true;
            //var sql = "UPDATE Video SET Title = @title, DatePublished= @DatePublished, IsActive= @IsActive, "
            //            + "WHERE VideoId = @VideoId";
            //db.Execute(sql, video, commandType: CommandType.Text);
        }
        public async Task<bool> DeleteVideoById(int videoId)
        {
            var parms = new DynamicParameters();
            parms.Add("Videoid", videoId, DbType.Int32);
            using (var con = new SqlConnection(_configuration.Value))
            {
                await con.ExecuteAsync("spVideo_DeleteVideo", parms, commandType: CommandType.StoredProcedure);
            }
            return true;
        }
    }
}
