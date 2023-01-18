#!/usr/bin/env bash
set -euo pipefail

aws_region="ap-southeast-2"
ecr_registry="274387265859.dkr.ecr.ap-southeast-2.amazonaws.com"
local_image="jonathan-shoppinglistapi"
image_tag=$(git rev-parse --short HEAD)

docker build -t "$local_image:$image_tag" .

docker tag "$local_image:$image_tag" "$ECR_REPO:$image_tag"

docker push "$ECR_REPO:$image_tag"
