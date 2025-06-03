<Query Kind="Statements">
  <NuGetReference>LanguageExt.Core</NuGetReference>
  <Namespace>LanguageExt</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

#load "e\GenericExtensions"
#load "e\DumpingNewType"
#load "e\ExceptionExtensions"

public abstract class FileSystemPath(string value) : DumpingNewType<FileSystemPath, string>(value);

public sealed class FileSystemDirectoryPath : FileSystemPath
{
	public FileSystemDirectoryPath(string directory) : base(directory)
	{
		if (!Path.EndsInDirectorySeparator(directory))
			throw new ArgumentException("Path to directory should end with directory separator.", nameof(directory))
				.WithData("DirectorySeparator", Path.DirectorySeparatorChar)
				.WithData("Argument", directory);
	}
}

public sealed class FileSystemFilePath : FileSystemPath
{
	public FileSystemFilePath(string file) : base(file)
	{
		if (Path.EndsInDirectorySeparator(file))
			throw new ArgumentException("Path to file should not end with directory separator.", nameof(file))
				.WithData("Argument", file);
	}
	
	public static implicit operator string(FileSystemFilePath p) => p.Value;
}

public sealed class FileSystemFileExtension(string value) : DumpingNewType<FileSystemFileExtension, string>(value);

public static class FileSystemExtensions
{
	public static bool Exists(this FileSystemDirectoryPath @this) => Directory.Exists(@this.Value);

	public static FileSystemDirectoryPath Join(this FileSystemDirectoryPath @this, FileSystemDirectoryPath next)
	{
		return Path
			.Join(@this.Value, next.Value)
			.ToDirectory();
	}

	public static FileSystemDirectoryPath JoinDirectory(this FileSystemDirectoryPath @this, FileSystemDirectoryPath next) => @this.Join(next);

	public static FileSystemFilePath Join(this FileSystemDirectoryPath @this, FileSystemFilePath next)
	{
		return Path
			.Join(@this.Value, next.Value)
			.ToFile();
	}

	public static FileSystemFilePath JoinFile(this FileSystemDirectoryPath @this, FileSystemFilePath next) => @this.Join(next);

	public static FileSystemDirectoryPath CreateIfNotExists(this FileSystemDirectoryPath @this)
	{
		if (!Directory.Exists(@this.Value))
			Directory.CreateDirectory(@this.Value);

		return @this;
	}

	public static bool Exists(this FileSystemFilePath @this) => File.Exists(@this.Value);

	public static IEnumerable<FileSystemFilePath> EnumerateFiles(
		this FileSystemDirectoryPath @this,
		string searchPattern,
		EnumerationOptions enumerationOptions)
	{
		return Directory
			.EnumerateFiles(
				@this.Value,
				searchPattern,
				enumerationOptions)
			.Select(file => file.ToFile());
	}

	public static IEnumerable<FileSystemFilePath> EnumerateFiles(
		this FileSystemDirectoryPath @this,
		string searchPattern)
	{
		return Directory
			.EnumerateFiles(
				@this.Value,
				searchPattern)
			.Select(file => file.ToFile());
	}

	public static IEnumerable<FileSystemDirectoryPath> EnumerateDirectories(
		this FileSystemDirectoryPath @this,
		string searchPattern,
		EnumerationOptions enumerationOptions)
	{
		return Directory
			.EnumerateDirectories(
				@this.Value,
				searchPattern,
				enumerationOptions)
			.Select(dir => dir.FixEndingDirectorySeparator().ToDirectory());
	}

	public static bool IsEmpty(this FileSystemDirectoryPath @this)
	{
		return Directory
			.EnumerateFileSystemEntries(
				@this.Value,
				"*",
				new EnumerationOptions() { RecurseSubdirectories = true })
			.Any() ? false : true;
	}

	public static void MoveTo(this FileSystemDirectoryPath @this, FileSystemDirectoryPath destination)
	{
		Directory.Move(@this.Value, destination.Value);
	}

	public static void MoveTo(this FileSystemFilePath @this, FileSystemFilePath newPath, bool overwrite = false)
	{
		File.Move(@this.Value, newPath.Value, overwrite);
	}

	public static void CopyTo(this FileSystemFilePath @this, FileSystemFilePath newPath)
	{
		File.Copy(@this.Value, newPath.Value);
	}

	public static void CopyTo(this FileSystemFilePath @this, FileSystemDirectoryPath destDir)
	{
		File.Copy(@this.Value, destDir.Join(@this.GetFileName()));
	}

	public static void Delete(this FileSystemFilePath @this)
	{
		File.Delete(@this);
	}

	public static void Delete(this FileSystemDirectoryPath @this, bool recursive = false)
	{
		Directory.Delete(@this.Value, recursive);
	}

	public static (Option<FileSystemFileExtension> Extension, string FileNameWithoutExtension) GetExtension(this FileSystemFilePath @this)
	{
		var extension = Prelude
			.Optional(Path.GetExtension(@this.Value))
			.Map(str => str.AsSpan()[1..].ToString())
			.Map(str => new FileSystemFileExtension(str));
		var withoutExtension = Path.GetFileNameWithoutExtension(@this.Value);
		return (extension, withoutExtension);
	}
	

	public static FileSystemFilePath GetFileName(this FileSystemFilePath @this)
	{
		return Path
			.GetFileName(@this.Value)
			.ToFile();
	}

	public static FileSystemDirectoryPath GetDirectoryName(this FileSystemFilePath @this)
	{
		return Path
			.GetDirectoryName(@this.Value)
			.Transform(dirname => dirname ?? throw new InvalidOperationException("Failed to get directory name."))
			.FixEndingDirectorySeparator()
			.ToDirectory();
	}

	public static FileSystemDirectoryPath GetRelativePathTo(this FileSystemDirectoryPath @this, FileSystemDirectoryPath relativeTo)
	{
		return Path
			.GetRelativePath(
				relativeTo.Value,
				@this.Value)
			.FixEndingDirectorySeparator()
			.ToDirectory();
	}

	public static FileSystemFilePath GetRelativePathTo(this FileSystemFilePath @this, FileSystemDirectoryPath relativeTo)
	{
		return Path
			.GetRelativePath(
				relativeTo.Value,
				@this.Value)
			.ToFile();
	}

	private static string FixEndingDirectorySeparator(this string @this) => Path.EndsInDirectorySeparator(@this) ? @this : @this + Path.DirectorySeparatorChar;

	private static FileSystemDirectoryPath ToDirectory(this string @this) => new(@this);

	private static FileSystemFilePath ToFile(this string @this) => new(@this);
}