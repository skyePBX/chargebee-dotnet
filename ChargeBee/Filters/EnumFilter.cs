using System;
using ChargeBee.Api;

namespace ChargeBee.Filters
{
    public class EnumFilter<T, TU> where TU : EntityRequest<TU>
    {
        private readonly string _paramName;

        private readonly TU _req;
        private bool _supportsPresenceOperator;

        public EnumFilter(string paramName, TU req)
        {
            _paramName = paramName;
            _req = req;
        }

        public EnumFilter<T, TU> SupportsPresenceOperator(bool supportsPresenceOperator)
        {
            _supportsPresenceOperator = supportsPresenceOperator;
            return this;
        }

        public TU Is(T value)
        {
            _req.Params().AddOpt(_paramName + "[is]", value);
            return _req;
        }

        public TU IsNot(T value)
        {
            _req.Params().AddOpt(_paramName + "[is_not]", value);
            return _req;
        }

        public TU In(params T[] value)
        {
            _req.Params().AddOpt(_paramName + "[in]", value);
            return _req;
        }

        public TU NotIn(params T[] value)
        {
            _req.Params().AddOpt(_paramName + "[not_in]", value);
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