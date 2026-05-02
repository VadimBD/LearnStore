Write-Host "=== Building image ==="
docker build -t learnstore-migrations:latest -f LearnStore.Database.Migrations/Dockerfile .
docker tag learnstore-migrations:latest localhost:5000/learnstore-migrations:latest
docker push localhost:5000/learnstore-migrations:latest
Write-Host "=== Deploying migrations ==="
docker stack deploy -c ./deploy/database-migrations.yml learnStore
Write-Host "=== Deployment complete ==="