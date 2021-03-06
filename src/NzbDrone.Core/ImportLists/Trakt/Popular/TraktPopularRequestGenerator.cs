using System.Collections.Generic;
using NzbDrone.Common.Http;
using NzbDrone.Core.Notifications.Trakt;

namespace NzbDrone.Core.ImportLists.Trakt.Popular
{
    public class TraktPopularRequestGenerator : IImportListRequestGenerator
    {
        private readonly ITraktProxy _traktProxy;
        public TraktPopularSettings Settings { get; set; }

        public TraktPopularRequestGenerator(ITraktProxy traktProxy)
        {
            _traktProxy = traktProxy;
        }

        public virtual ImportListPageableRequestChain GetMovies()
        {
            var pageableRequests = new ImportListPageableRequestChain();

            pageableRequests.Add(GetMoviesRequest());

            return pageableRequests;
        }

        private IEnumerable<ImportListRequest> GetMoviesRequest()
        {
            var link = string.Empty;

            var filtersAndLimit = $"?years={Settings.Years}&genres={Settings.Genres.ToLower()}&ratings={Settings.Rating}&certifications={Settings.Certification.ToLower()}&limit={Settings.Limit}{Settings.TraktAdditionalParameters}";

            switch (Settings.TraktListType)
            {
                case (int)TraktPopularListType.Trending:
                    link += "movies/trending" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.Popular:
                    link += "movies/popular" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.Anticipated:
                    link += "movies/anticipated" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.BoxOffice:
                    link += "movies/boxoffice" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.TopWatchedByWeek:
                    link += "movies/watched/weekly" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.TopWatchedByMonth:
                    link += "movies/watched/monthly" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.TopWatchedByYear:
                    link += "movies/watched/yearly" + filtersAndLimit;
                    break;
                case (int)TraktPopularListType.TopWatchedByAllTime:
                    link += "movies/watched/all" + filtersAndLimit;
                    break;
            }

            var request = new ImportListRequest(_traktProxy.BuildTraktRequest(link, HttpMethod.GET, Settings.AccessToken));

            yield return request;
        }
    }
}
