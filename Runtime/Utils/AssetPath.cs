namespace Library
{
    public class AssetPath
    {
        private const string ResourcesFolderName = "/Resources/";


        /// <summary>
        /// Takes the string from the Asset Path Attribute and converts it into
        /// a usable resources path.
        /// </summary>
        /// <param name="assetPath">The project path that AssetPathAttribute serializes</param>
        /// <returns>The resources path if it exists otherwise returns the same path</returns>
        public static string ConvertToResourcesPath(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                return string.Empty;
            }

            int folderIndex = projectPath.IndexOf(ResourcesFolderName);

            if (folderIndex == -1)
            {
                return string.Empty;
            }

            folderIndex += ResourcesFolderName.Length;
            int length = projectPath.Length - folderIndex;
            length -= projectPath.Length - projectPath.LastIndexOf('.');

            string resourcesPath = projectPath.Substring(folderIndex, length);

            return resourcesPath;
        }
    }
}