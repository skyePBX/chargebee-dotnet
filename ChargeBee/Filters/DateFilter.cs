using System;
using ChargeBee.Api;

namespace ChargeBee.Filters
{
    public class DateFilter<TU> where TU : EntityRequest<TU>
    {
        private readonly string _paramName;

        private readonly TU _req;
        private bool _supportsPresenceOperator;

        public DateFilter(string paramName, TU req)
        {
            _paramName = paramName;
            _req = req;
        }

        public DateFilter<TU> SupportsPresenceOperator(bool supportsPresenceOperator)
        {
            _supportsPresenceOperator = supportsPresenceOperator;
            return this;
        }

        public TU On(DateTime value)
        {
            _req.Params().AddOpt(_paramName + "[on]", value, true);
            return _req;
        }

        public TU Before(DateTime value)
        {
            _req.Params().AddOpt(_paramName + "[before]", value, true);
            return _req;
        }

        public TU After(DateTime value)
        {
            _req.Params().AddOpt(_paramName + "[after]", value, true);
            return _req;
        }

        public TU Between(DateTime value1, DateTime value2)
        {
            _req.Params().AddOpt(_paramName + "[between]", new[] {value1, value2}, true);
            return _req;
        }

        public TU IsPresent(bool value)
        {
            if (!_supportsPresenceOperator)
                throw new NotSupportedException("operator '[is_present]' is not supported for this filter-parameter");
            _req.Params().AddOpt(_paramName + "[is_present]", value);
            return _req;
        }
    }
}