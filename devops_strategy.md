# DevOps Branching and Deployment Strategy

## Single-Branch Environment Strategy

- Use only the `main` branch for all deployments (dev, staging, prod).
- All code changes are merged to `main` via pull requests with required reviews and CI checks.
- Deployments to dev, staging, and prod are triggered from the `main` branch, but use different configuration overlays (Kustomize, Helm, or environment variables) for each environment.
- Use Git tags to mark production releases (e.g., v1.0.0).
- Use feature branches only for development, and delete them after merging to `main`.

## Example Workflow

1. Developer creates a feature branch from `main`.
2. Work is completed and a pull request is opened to `main`.
3. After review and CI pass, the PR is merged to `main`.
4. CI/CD pipeline automatically deploys to dev and/or staging using the latest `main`.
5. When ready, promote the same commit to production (optionally using a tag or manual approval).

## Key Points
- All environments always run code from the same branch.
- Environment differences are managed by configuration, not by code divergence.
- Promotes consistency, reduces merge conflicts, and simplifies release management.
