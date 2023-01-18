#!/usr/bin/env bash
set -euo pipefail

image_tag=$(git rev-parse --short HEAD)

ktmpl ops/deploy/template.yaml \
  --parameter app "jonathan-shoppinglistapi" \
  --parameter namespace "fma" \
  --parameter host "jonathan-shoppinglistapi.svc.platform.myobdev.com" \
  --parameter image "$ECR_REPO" \
  --parameter imageTag "$image_tag" \
  --parameter containerPort "80" \
  --parameter db_name "shoppinglist-db" \
  --parameter db_instance "db.t4g.small" \
  --parameter db_storage "20" \
  | kubectl apply -f -
