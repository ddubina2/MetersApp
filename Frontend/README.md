# Premmplus portal UI 

## Requirements

- pnpm@9.x.x^ installed ([npm install -g pnpm](https://pnpm.io/installation))
- nodejs 20.x.x+

## Setup local development

1. Fill envs in `.env.local` use `.env.example` as a reference
2. Run `pnpm install` for dependencies installation
3. Run `pnpm dev` for local development

## Available commands

### General:

- `pnpm types` - generate types from back-end swagger

### Linting

- To linting and formatting `pnpm lint:fix`. To check linting without fixing: `pnpm lint:check`

### Build for production

- Run `pnpm build`
- Optional: for previewing builded project run `pnpm preview`
