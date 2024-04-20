using ClassLibrary;

namespace ClassLibraryTests
{
    public class AsyncLibraryTests
    {
        [Test]
        public void AwaiterTest()
        {
            AsyncLibrary library = new AsyncLibrary();
            library.RunAwaiter();
        }

        [Test]
        public void AsyncTest()
        {
            AsyncLibrary library = new AsyncLibrary();
            var result = library.RunAsync().GetAwaiter().GetResult();
        }

        [Test]
        public void AsyncWhenAllTest()
        {
            AsyncLibrary library = new AsyncLibrary();
            var result = library.RunWhenAllAsync().GetAwaiter().GetResult();
        }
    }
}