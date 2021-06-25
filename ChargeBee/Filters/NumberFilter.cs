using System;
using ChargeBee.Api;

namespace ChargeBee.Filters
{
    public class NumberFilter<T, TU> where TU : EntityRequest<TU>
    {
        private readonly string _paramName;
        private readonly TU _req;
        private bool _supportsPresenceOperator;

        public NumberFilter(string paramName, TU req)
        {
            _paramName = paramName;
            _req = req;
        }

        public NumberFilter<T, TU> SupportsPresenceOperator(bool supportsPresenceOperator)
        {
            _supportsPresenceOperator = supportsPresenceOperator;
            return this;
        }

        public TU Gt(T value)
        {
            _req.Params().AddOpt(_paramName + "[gt]", value);
            return _req;
        }

        public TU Lt(T value)
        {
            _req.Params().AddOpt(_paramName + "[lt]", value);
            return _req;
        }

        public TU Gte(T value)
        {
            _req.Params().AddOpt(_paramName + "[gte]", value);
            return _req;
        }

        public TU Lte(T value)
        {
            _req.Params().AddOpt(_paramName + "[lte]", value);
            return _req;
        }

        public TU Between(T val1, T val2)
        {
            _req.Params().AddOpt(_paramName + "[between]", new[] {val1, val2});
            return _req;
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

        public TU IsPresent(bool value)
        {
            if (!_supportsPresenceOperator)
                throw new NotSupportedException("operator '[is_present]' is not supported for this filter-parameter");

            _req.Params().AddOpt(_paramName + "[is_present]", value);
            return _req;
        }
    }
}