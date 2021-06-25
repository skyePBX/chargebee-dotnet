using System;
using ChargeBee.Api;

namespace ChargeBee.Filters
{
    public class StringFilter<TU> where TU : EntityRequest<TU>
    {
        private readonly string _paramName;
        private readonly TU _req;
        private bool _supportsMultiOperator;
        private bool _supportsPresenceOperator;

        public StringFilter(string paramName, TU req)
        {
            _paramName = paramName;
            _req = req;
            _supportsPresenceOperator = true;
        }

        public StringFilter<TU> SupportsPresenceOperator(bool supportsPresenceOperators)
        {
            _supportsPresenceOperator = supportsPresenceOperators;
            return this;
        }

        public StringFilter<TU> SupportsMultiOperators(bool supportsMultiOperators)
        {
            _supportsMultiOperator = supportsMultiOperators;
            return this;
        }

        public TU Is(string value)
        {
            _req.Params().AddOpt(_paramName + "[is]", value);
            return _req;
        }


        public TU IsNot(string value)
        {
            _req.Params().AddOpt(_paramName + "[is_not]", value);
            return _req;
        }

        public TU StartsWith(string value)
        {
            _req.Params().AddOpt(_paramName + "[starts_with]", value);
            return _req;
        }

        public TU IsPresent(bool value)
        {
            if (!_supportsPresenceOperator)
                throw new NotSupportedException("operator '[is_present]' is not supported for this filter-parameter");
            _req.Params().AddOpt(_paramName + "[is_present]", value);
            return _req;
        }

        public TU In(params string[] value)
        {
            if (!_supportsMultiOperator)
                throw new NotSupportedException("operator '[in]' is not supported for this filter-parameter");

            _req.Params().AddOpt(_paramName + "[in]", value);
            return _req;
        }

        public TU NotIn(params string[] value)
        {
            if (!_supportsMultiOperator)
                throw new NotSupportedException("operator '[not_in]' is not supported for this filter-parameter");
            _req.Params().AddOpt(_paramName + "[not_in]", value);
            return _req;
        }
    }
}