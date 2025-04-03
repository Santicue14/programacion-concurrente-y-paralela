import os

class Config:
    SECRET_KEY = "Sup3r$ecretK3y"
    SQLALCHEMY_DATABASE_URI = "sqlite:///tasks.db"
    SQLALCHEMY_TRACK_MODIFICATIONS = False
