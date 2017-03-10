using System;

public enum BindingFlags
{
	Public = 1,
	NonPublic,
	Instance = 4,
	SetProperty = 8,
	GetProperty = 16,
	Static = 32,
	IgnoreCase = 64,
	FlattenHierarchy = 128
}
