Write-Host "=== Building image ==="
docker build -t learnstore-fake-payment-gatewayt:latest -f LearnStore.FakePaymentGateway/Dockerfile .
docker tag learnstore-fake-payment-gatewayt:latest localhost:5000/learnstore-fake-payment-gateway:latest
docker push localhost:5000/learnstore-fake-payment-gateway:latest
Write-Host "=== Deploying api ==="
docker stack deploy -c ./deploy/fake-payment-gateway.yml learnStore
Write-Host "=== Deployment complete ==="