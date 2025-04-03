from flask import Blueprint, request, jsonify
from app import db, bcrypt
from app.models import User, Task
from flask_jwt_extended import create_access_token, jwt_required, get_jwt_identity, get_jwt
from app import jwt
from datetime import datetime
from datetime import timedelta

api = Blueprint("api", __name__)

blacklist = set()

@api.route("/logout", methods=["POST"])
@jwt_required()
def logout():
    jti = get_jwt()["jti"]  # Obtén el ID único del token actual
    blacklist.add(jti)
    return jsonify(message="Token revoked, user logged out"), 200

@jwt.token_in_blocklist_loader
def check_if_token_is_revoked(jwt_header, jwt_payload):
    return jwt_payload["jti"] in blacklist
    
@api.route("/register", methods=["POST"])
def register():
    data = request.json
    if User.query.filter_by(username=data["username"]).first():
        return jsonify(message="Username already exists"), 400

    hashed_pw = bcrypt.generate_password_hash(data["password"]).decode("utf-8")
    user = User(username=data["username"], password=hashed_pw)
    db.session.add(user)
    db.session.commit()
    return jsonify(message="User registered successfully"), 201

@api.route("/login", methods=["POST"])
def login():
    data = request.json
    user = User.query.filter_by(username=data["username"]).first()
    if user and bcrypt.check_password_hash(user.password, data["password"]):
        
        # identity = {"id": str(user.id), "username": user.username}
        # token = create_access_token(identity=identity, expires_delta=expires)
        # token = create_access_token(identity=str(user.id), expires_delta=expires)
        
        identity = str(user.id)               
        expires = timedelta(hours=5)
        
        # Create the token
        token = create_access_token(
            identity=identity,
            expires_delta=expires,
            additional_claims={"username": user.username}  
        )
        return jsonify(access_token=token), 200
    return jsonify(message="Invalid credentials"), 401

@api.route("/tasks", methods=["GET", "POST"])
@jwt_required()
def tasks():
    # user_id = str(get_jwt_identity())
    identity = get_jwt_identity()
    user_id = identity
    print(f"identity: {user_id}")
    if request.method == "GET":
        tasks = Task.query.filter_by(user_id=user_id).all()
        return jsonify([{
            "id": task.id,
            "title": task.title,
            "description": task.description,
            "completed": task.completed,
            "due_date": task.due_date,
            "user_id": task.user_id
        } for task in tasks]), 200

    data = request.json
    due_date_str = data.get('due_date')
    due_date = None
    if due_date_str:
        due_date = datetime.fromisoformat(due_date_str)  # Convierte la cadena a datetime

    task = Task(
        title = data["title"],
        description = data.get("description"),
        completed = data.get("completed", False),  # Default a False si no se proporciona
        due_date = due_date,
        user_id = user_id
    )
    db.session.add(task)
    db.session.commit()
    return jsonify(message="Task created"), 201

@api.route("/tasks/<int:id>", methods=["GET"])
@jwt_required()  # Protege la ruta para usuarios autenticados
def get_task_by_id(id):
    task = Task.query.get(id)  # Busca la tarea por ID
    if task:
        return jsonify({
            'id': task.id,
            'title': task.title,
            'description': task.description,
            'completed': task.completed,
            'due_date': task.due_date,
            'user_id': task.user_id
        }), 200
    else:
        return jsonify(message="Task not found"), 404

@api.route("/tasks/<int:id>", methods=["PUT"])
@jwt_required()
def update_task(id):
    data = request.json
    
    # Buscar tarea por ID
    task = Task.query.get(id)
    if not task:
        return jsonify(message="Task not found"), 404

    due_date_str = data.get('due_date')
    due_date = None
    if due_date_str:
        due_date = datetime.fromisoformat(due_date_str)  # Convierte la cadena a datetime
        
    # Actualizar tarea con los nuevos datos
    task.title = data.get("title", task.title)
    task.description = data.get("description", task.description)
    task.completed = data.get("completed", task.completed)
    task.due_date = due_date = datetime.strptime(data.get('due_date'), '%Y-%m-%dT%H:%M')
    
    # Guardar cambios en la base de datos
    db.session.commit()
    
    return jsonify(message="Task updated successfully"), 200

@api.route('/tasks/<int:task_id>', methods=['DELETE'])
def delete_task(task_id):
    task = Task.query.get(task_id)
    if not task:
        return jsonify({'error': 'Task not found'}), 404
    try:
        db.session.delete(task)
        db.session.commit()
        return jsonify({'message': 'Task deleted successfully'}), 200
    except Exception as e:
        db.session.rollback()
        return jsonify({'error': 'Error deleting task', 'details': str(e)}), 500

