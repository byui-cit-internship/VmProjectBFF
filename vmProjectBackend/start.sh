docker build -t vmprojectbackend .
docker run -p 5000:80 -e ASPNETCORE_ENVIRONMENT=Development vmprojectbackend