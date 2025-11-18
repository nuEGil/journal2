Install visual studio for the package manager and to get all the dotnet goodies then never open it again. 

dotnet workload list

dotnet workload install maui 
dotnet workload install android
dotnet build -t:Run -f:net9.0-android