using AutoMapper;
using HappyShop.Documents;
using HappyShop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HappyShop.Comm
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutoMapperExtension
    {
        public static IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        static AutoMapperExtension()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInfoEntity, UserInfoDocument>();
                cfg.CreateMap<UserInfoDocument, UserInfoEntity>();

                cfg.CreateMap<UserInfoEntity, Model.UserInfo>();
            });
            _mapper = config.CreateMapper();
        }


        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Convert<S, T>(this IEnumerable<S> list)
        {
            if (list == null)
            {
                return null;
            }
            return list.Select(d => _mapper.Map<S, T>(d)).ToList();
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Convert<S, T>(this IQueryable<S> list)
        {
            if (list == null)
            {
                return null;
            }
            return list.Select(d => _mapper.Map<S, T>(d)).ToList();
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Convert<S, T>(this IList<S> list)
        {
            if (list == null)
            {
                return null;
            }
            return list.Select(d => _mapper.Map<S, T>(d)).ToList();
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T Convert<S, T>(this S model)
        {
            if (model == null)
            {
                return default(T);
            }
            return _mapper.Map<S, T>(model);
        }
    }
}
