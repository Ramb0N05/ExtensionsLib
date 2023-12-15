using System;

#if !NET20 && !NET35 && !NET40

using System.ComponentModel.DataAnnotations;

#endif

namespace SharpRambo.ExtensionsLib.EnumExtensions {

    public class EnumFieldEntry {

        #region Public Properties

        public string Category { get; }
        public string Description { get; }
        public string DisplayName { get; set; }
        public Enum EnumObject { get; }
        public string Name { get; }

#if !NET20 && !NET35 && !NET40
        public DisplayAttribute Display { get; }
#endif

        #endregion Public Properties

        #region Public Constructors

        public EnumFieldEntry(Enum enumVal) : this(enumVal.GetName(), enumVal.GetDescription(), enumVal) {
        }

#if !NET20 && !NET35 && !NET40

        public EnumFieldEntry(string name, Enum enumVal) : this(name, string.Empty, enumVal) {
        }

#endif

        public EnumFieldEntry(string name, string description, Enum enumVal) {
            if (name.IsNull())
                throw new ArgumentNullException(nameof(name));

            EnumObject = enumVal ?? throw new ArgumentNullException(nameof(enumVal));

            Name = name;
            Description = description;
            DisplayName = name;
            Category = enumVal.GetCategory();

#if !NET20 && !NET35 && !NET40
            Display = enumVal.GetDisplay();

            if (Display?.Description.IsNull() == false && description.IsNull())
                Description = Display.Description;
#endif
        }

        #endregion Public Constructors
    }
}
