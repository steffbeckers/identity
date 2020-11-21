# build stage
FROM node:lts as build-stage

WORKDIR /app

COPY package*.json ./

RUN npm install

RUN npm rebuild node-sass

RUN ./node_modules/.bin/ngcc --properties es2015

COPY . .

RUN npm run build

# production stage
FROM nginx as production-stage

COPY --from=build-stage /app/dist/angular /usr/share/nginx/html

COPY --from=build-stage /app/nginx.conf /etc/nginx/conf.d/default.conf
