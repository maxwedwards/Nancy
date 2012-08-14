namespace Nancy.ModelBinding
{
    using System;
    using System.ComponentModel;

    public static class ModuleExtensions
    {
        /// <summary>
        /// Bind the incoming request to a model
        /// </summary>
        /// <param name="module">Current module</param>
        /// <param name="blacklistedProperties">Property names to blacklist from binding</param>
        /// <returns>Model adapter - cast to a model type to bind it</returns>
        public static dynamic Bind(this NancyModule module, params string[] blacklistedProperties)
        {
            return new DynamicModelBinderAdapter(module.ModelBinderLocator, module.Context, blacklistedProperties);
        }

        /// <summary>
        /// Bind the incoming request to a model
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="module">Current module</param>
        /// <param name="blacklistedProperties">Property names to blacklist from binding</param>
        /// <returns>Bound model instance</returns>
        public static TModel Bind<TModel>(this NancyModule module, params string[] blacklistedProperties)
        {
            return module.Bind(blacklistedProperties);
        }

        /// <summary>
        /// Bind the incoming request to an existing instance
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <param name="module">Current module</param>
        /// <param name="instance">The class instance to bind properties to</param>
        /// <param name="blacklistedProperties">Property names to blacklist from binding</param>
        public static void BindTo<TModel>(this NancyModule module, TModel instance, params string[] blacklistedProperties)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "The instance parameter cannot be null");
            }

            TModel boundModel = module.Bind(blacklistedProperties);

            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(boundModel))
            {
                var value = item.GetValue(boundModel);
                if (value != null)
                {
                    item.SetValue(instance, value);
                }
            }

        }
    }
}