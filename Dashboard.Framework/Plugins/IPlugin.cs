using NoeticTools.TeamStatusBoard.Framework.Services;


namespace NoeticTools.TeamStatusBoard.Framework.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        ///     <para>
        ///         Rank used to order the registration of plugins. Plugins are used in the same order as registration with the
        ///         first found being used. This allows new plugins to effectively replace built-in plugins.
        ///     </para>
        ///     <para>The higher the rank the earlier the plugin is registered. Built-in plugins have a rank of 0.</para>
        /// </summary>
        int Rank { get; }

        void Register(IServices services);
    }
}