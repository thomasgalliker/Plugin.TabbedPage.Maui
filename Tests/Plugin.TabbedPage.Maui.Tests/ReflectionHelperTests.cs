using Plugin.TabbedPage.Maui.Utils;
using Xunit;

namespace Plugin.TabbedPage.Maui.Tests
{
    public class ReflectionHelperTests
    {
        [Fact]
        public void ShouldGetFieldValue()
        {
            // Arrange
            var obj = new MyClass(internalDateTime: null, internalString: "test value");

            // Act
            var FieldValue = ReflectionHelper.GetFieldValue<string>(obj, "InternalString");

            // Assert
            Assert.Equal(obj.InternalString, FieldValue);
        }

        [Fact]
        public void ShouldGetFieldValue_FromBaseClass()
        {
            // Arrange
            var obj = new MyClass(internalDateTime: new DateTime(2000, 1, 1), internalString: null);

            // Act
            var FieldValue = ReflectionHelper.GetFieldValue<DateTime>(obj, "InternalDateTime");

            // Assert
            Assert.Equal(obj.InternalDateTime, FieldValue);
        }

        [Fact]
        public void ShouldGetFieldValue_FieldDoesNotExist()
        {
            // Arrange
            var obj = new MyClass(internalDateTime: null, internalString: null);

            // Act
            Action action = () => ReflectionHelper.GetFieldValue<string>(obj, "NonExistentField");

            // Assert
            Assert.Throws<ArgumentException>(action);
        }

        public class MyClass : MyBaseClass
        {
            public MyClass(DateTime? internalDateTime, string internalString)
                : base(internalDateTime)
            {
                this.InternalString = internalString;
            }

            internal readonly string InternalString;
        }

        public class MyBaseClass
        {
            protected MyBaseClass(DateTime? internalDateTime)
            {
                this.InternalDateTime = internalDateTime;
            }

            internal DateTime? InternalDateTime;
        }
    }
}