using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace AutoMapperIgnoreProperty.Extensions
{
    public static class IgnoreAllValueNullExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNull<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(options =>
            {
                options.Condition((src, dest, srcMember) => srcMember != null);
            });
            return expression;
        }
    }
}