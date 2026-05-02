Write-Host "=== Building image ==="
docker build -t learnstore-api:latest -f LearnStore.Web.Api/Dockerfile .
docker tag learnstore-api:latest localhost:5000/learnstore-api:latest
docker push localhost:5000/learnstore-api:latest
Write-Host "=== Deploying api ==="
docker stack deploy -c ./deploy/api.yml learnStore
Write-Host "=== Deployment complete ==="