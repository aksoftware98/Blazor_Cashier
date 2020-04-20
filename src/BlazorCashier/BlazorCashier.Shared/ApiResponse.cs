using System;
using System.Collections.Generic;

namespace BlazorCashier.Shared
{
    public class ApiResponse
    {
        public bool IsSuccess => string.IsNullOrEmpty(Error);

        public string Error { get; }

        public ApiResponse(string error = null)
        {
            Error = error;
        }
    }

    public class EntityApiResponse<TEntity> : ApiResponse where TEntity : class
    {
        public TEntity Entity { get; set; }

        public EntityApiResponse(TEntity entity = null, string error = null)
            : base(error)
        {
            Entity = entity;
        }
    }

    public class EntitiesApiResponse<TEntity> : ApiResponse where TEntity : class
    {
        public IEnumerable<TEntity> Entities { get; set; }

        public EntitiesApiResponse(IEnumerable<TEntity> entities = null, string error = null) 
            : base(error)
        {
            Entities = entities;
        }
    }
    
    public class EntitiesPagingApiResponse<TEntity> : EntitiesApiResponse<TEntity> where TEntity : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }

        public EntitiesPagingApiResponse(int totalResults = 0, IEnumerable<TEntity> entities = null, int pageNumber = 0, int pageSize = 10, string error = null)
            : base(entities, error)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalResults = totalResults;
        }
    }

    public class IdentityApiResponse : ApiResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpireDate { get; set; }

        public IdentityApiResponse(string error = null) : base(error) { }

        public IdentityApiResponse(string accessToken, DateTime expireDate)
        {
            AccessToken = accessToken;
            ExpireDate = expireDate;
        }
    }
}
