using System.Buffers;

namespace StuceSoftware.Utilities.Test;

public class TemporaryFileTests
{
    [Fact]
    public void GivenNewTemporaryFile_WhenConstructed_CreatesNewEmptyFile()
    {
        using var tempFile = new TemporaryFile();

        Assert.True(File.Exists(tempFile.FileName));
        Assert.Equal(0, new FileInfo(tempFile.FileName).Length);
    }

    [Fact]
    public void GivenNewTemporaryFile_WithSpecificExtension_CreatesTempFileWithExpectedExtension()
    {
        const string expectedExtension = ".ext";
        using var tempFile = new TemporaryFile(expectedExtension);

        Assert.Equal(expectedExtension, Path.GetExtension(tempFile.FileName));
    }

    [Fact]
    public void GivenTemporaryFile_WhenDisposed_DeletesFile()
    {
        var tempFile = new TemporaryFile();

        Assert.True(File.Exists(tempFile.FileName));

        tempFile.Dispose();
        Assert.False(File.Exists(tempFile.FileName));
    }

    [Fact]
    public void GivenNewTemporaryFile_WithSpecifiedNonExistingDirectory_CreatesDirectory()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Assert.False(Directory.Exists(tempPath));

        using (_ = new TemporaryFile(directory: tempPath))
        {
            Assert.True(Directory.Exists(tempPath));
        }

        Directory.Delete(tempPath, true);
    }

    [Fact]
    public async void GivenTemporaryFile_WhenAsyncDisposed_DeletesFile()
    {
        var fileName = await CreateTempFileAsynchronously().ConfigureAwait(false);
        Assert.False(File.Exists(fileName));
    }

    // Helper method to verify AsyncDispose is called correctly
    private static async Task<string> CreateTempFileAsynchronously()
    {
        await using var tempFile = new TemporaryFile();
        var buffer = new byte[128];
        Random.Shared.NextBytes(buffer);
        await File.WriteAllBytesAsync(tempFile.FileName, buffer).ConfigureAwait(false);

        Assert.True(File.Exists(tempFile.FileName));

        return tempFile.FileName;
    }
}