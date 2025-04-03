from flask import Flask
from flask_sqlalchemy import SQLAlchemy
from flask_bcrypt import Bcrypt
from flask_jwt_extended import JWTManager
from flask_cors import CORS
from app.config import Config

db = SQLAlchemy()
bcrypt = Bcrypt()
jwt = JWTManager()

def create_app():
    app = Flask(__name__)
    
    # Configuración de Flask-JWT-Extended
    app.config['JWT_SECRET_KEY'] = 'Secret0_$uper_segur0!'
    jwt = JWTManager(app)
    
    app.config.from_object(Config)

    db.init_app(app)
    bcrypt.init_app(app)
    jwt.init_app(app)
    # CORS(app)
    CORS(app, resources={r"/*": {"origins": "http://localhost:4200"}})

    from app.routes import api
    app.register_blueprint(api)

    return app
