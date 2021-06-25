using System.Threading.Tasks;
using ChargeBee.Filters;

namespace ChargeBee.Api
{
    public class ListRequestBase<TU> : EntityRequest<TU> where TU : ListRequestBase<TU>
    {
        public ListRequestBase(string url) : base(url, HttpMethod.Get)
        {
        }

        public TU Limit(int limit)
        {
            MParams.AddOpt("limit", limit);
            return (TU) this;
        }

        public TU Offset(string offset)
        {
            MParams.AddOpt("offset", offset);
            return (TU) this;
        }

        public StringFilter<TU> StringFilterParam(string paranName)
        {
            return new StringFilter<TU>(paranName, (TU) this).SupportsMultiOperators(true).SupportsPresenceOperator(true);
        }

        public BooleanFilter<TU> BooleanFilterParam(string paramName)
        {
            return new BooleanFilter<TU>(paramName, (TU) this).SupportsPresenceOperator(true);
        }

        public NumberFilter<long, TU> LongFilterParam(string paramName)
        {
            return new NumberFilter<long, TU>(paramName, (TU) this).SupportsPresenceOperator(true);
        }


        public TimestampFilter<TU> TimestampFilterParam(string paramName)
        {
            return new TimestampFilter<TU>(paramName, (TU) this).SupportsPresenceOperator(true);
        }

        public DateFilter<TU> DateFilterParam(string paramName)
        {
            return new DateFilter<TU>(paramName, (TU) this).SupportsPresenceOperator(true);
        }

        public new ListResult Request(ApiConfig env)
        {
            return ApiUtil.GetList(MUrl, MParams, Headers, env);
        }

        public new Task<ListResult> RequestAsync(ApiConfig env)
        {
            return ApiUtil.GetListAsync(MUrl, MParams, Headers, env);
        }

        public new ListResult Request()
        {
            return Request(ApiConfig.Instance);
        }

        public new Task<ListResult> RequestAsync()
        {
            return RequestAsync(ApiConfig.Instance);
        }
    }
}