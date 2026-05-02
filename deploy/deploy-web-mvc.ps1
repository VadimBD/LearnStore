Write-Host "=== Building image ==="
docker build -t learnstore-mvc:latest -f LearnStore.Web.MVC/Dockerfile .
docker tag learnstore-mvc:latest localhost:5000/learnstore-mvc:latest
docker push localhost:5000/learnstore-mvc:latest
Write-Host "=== Deploying applications ==="
docker stack deploy -c ./deploy/web-mvc.yml learnStore
Write-Host "=== Deployment complete ==="