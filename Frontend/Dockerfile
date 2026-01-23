FROM node:23.11.0-alpine3.21 AS build

# Preparations
ARG VITE_API_BASE_URL
ARG VITE_SENSORS_HUB_URL
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
ENV VITE_SENSORS_HUB_URL=$VITE_SENSORS_HUB_URL

RUN corepack enable
WORKDIR /app

# Install dependencies
COPY package.json pnpm-lock.yaml ./
RUN --mount=type=cache,id=pnpm,target=/root/.local/share/pnpm/store pnpm fetch --frozen-lockfile --ignore-scripts
RUN --mount=type=cache,id=pnpm,target=/root/.local/share/pnpm/store pnpm install --frozen-lockfile --ignore-scripts

# Build app
COPY . .
RUN pnpm exec vite build

# Prepare and start nginx
FROM nginx:alpine
COPY .nginx/nginx.conf /etc/nginx/nginx.conf
RUN rm -rf /usr/share/nginx/html/*

COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 3000 80

ENTRYPOINT ["nginx", "-g", "daemon off;"]
