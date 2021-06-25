using System;
using ChargeBee.Api;

namespace ChargeBee.Filters
{
    public class BooleanFilter<TU> where TU : EntityRequest<TU>
    {
        private readonly string _paramName;
        private readonly TU _req;
        private bool _supportsPresenceOperator;

        public BooleanFilter(string paramName, TU req)
        {
            _paramName = paramName;
            _req = req;
        }

        public TU Is(bool value)
        {
            _req.Params().AddOpt(_paramName + "[is]", value);
            return _req;
        }

        public BooleanFilter<TU> SupportsPresenceOperator(bool supportsPresenceOperator)
        {
            _supportsPresenceOperator = supportsPresenceOperator;
            return this;
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