get-job | remove-job -force

start-job {dotnet run -p ./src/WebApi}
start-job {dotnet run -p ./src/BlazorApp}

start-process "http://localhost:5000"
