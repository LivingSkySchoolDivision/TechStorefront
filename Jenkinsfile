pipeline {
    agent any
    environment {
        REPO = "techstorefront/storefront"
        PRIVATE_REPO = "${PRIVATE_DOCKER_REGISTRY}/${REPO}"
        PRIVATE_REPO_MAN = "${PRIVATE_DOCKER_REGISTRY}/${REPO}-manager"
        TAG = "j-${env.BUILD_NUMBER}"
    }
    stages {
        stage('Git clone') {
            steps {
                git branch: 'master',
                    credentialsId: 'JENKINS-AZUREDEVOPS',
                    url: 'git@ssh.dev.azure.com:v3/LivingSkySchoolDivision/LSSDStoreFront/LSSDStoreFront'
            }
        }
        stage('Docker build') {            
            steps {
                parallel(
                    FrontEnd: {
                        dir("LSSDStoreFront") {
                            sh "docker build --no-cache -t ${PRIVATE_REPO}:latest -f Dockerfile-Frontend -t ${PRIVATE_REPO}:${TAG} ."
                        }   
                    },
                    Manager: {
                        dir("LSSDStoreFront") {
                            sh "docker build --no-cache -t ${PRIVATE_REPO_MAN}:latest -f Dockerfile-Manager -t ${PRIVATE_REPO_MAN}:${TAG} ."
                        } 
                    }
                )                                 
            }
        }
        stage('Docker push') {
            steps {
                sh "docker push ${PRIVATE_REPO}:${TAG}"
                sh "docker push ${PRIVATE_REPO}:latest" 
                sh "docker push ${PRIVATE_REPO_MAN}:${TAG}"
                sh "docker push ${PRIVATE_REPO_MAN}:latest"           
            }
        }
    }
    post {        
        always {
            deleteDir()
        }
    }
}