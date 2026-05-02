Write-Host "=== Building image ==="
docker build -t learnstore-admin:latest -f LearnStore.Admin/Dockerfile .
docker tag learnstore-admin:latest localhost:5000/learnstore-admin:latest
docker push localhost:5000/learnstore-admin:latest
Write-Host "=== Deploying admin ==="
docker stack deploy -c ./deploy/admin.yml learnStore
Write-Host "=== Deployment complete ==="