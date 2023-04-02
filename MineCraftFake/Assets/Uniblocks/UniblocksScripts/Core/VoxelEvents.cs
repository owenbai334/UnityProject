using UnityEngine;
using System.Collections;

// inherit from this class if you don't want to use the default events

namespace Uniblocks {

public class VoxelEvents : MonoBehaviour {

	public virtual void OnMouseDown ( int mouseButton, VoxelInfo voxelInfo ) {
	
	}
	
	public virtual void OnMouseUp ( int mouseButton, VoxelInfo voxelInfo ) {
		
	}
	
	public virtual void OnMouseHold ( int mouseButton, VoxelInfo voxelInfo ) {
		
	}
	
	public virtual void OnLook ( VoxelInfo voxelInfo ) {
	
	}
	
	public virtual void OnBlockPlace ( VoxelInfo voxelInfo ) {
		
	}
	
	public virtual void OnBlockDestroy ( VoxelInfo voxelInfo ) {
	
	}		
	public virtual void OnBlockChange ( VoxelInfo voxelInfo ) {
	
	}
	
	public virtual void OnBlockEnter ( GameObject enteringObject, VoxelInfo voxelInfo ) {
	
	}
	
	public virtual void OnBlockStay ( GameObject stayingObject, VoxelInfo voxelInfo ) {
	
	}
}

}